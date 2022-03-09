#!/usr/bin/perl -w

use strict;
use utf8;
#binmode(STDOUT, ":utf8");
#binmode(STDERR, ":utf8");
use Data::Dumper;
my $debug = 1;

my $lang =
{
  'keywords' => [qw(IMy.+? Color Program Main abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while)],
  'var_keywords' => [qw(IMy.+? Color bool byte char class const decimal double enum explicit extern fixed float implicit int interface long namespace in out private protected public ref sbyte short static string struct uint ulong ushort void)],
  'space'  => [" ", "\t", "\n", "\r"],
  'punctuation' => ['(', ')', '[', ']', '{', '}', '*', ',', ':', '=', ';', '!', '@', '$', '%', '^', '&', '+', '-', '/', '.', '|', '?', '\'', '"'],
  'digit' => [qw(0 1 2 3 4 5 6 7 8 9)],
};
my @alphabet = (('a'..'z'), ('A'..'Z'));
my $max_line_size = 128;
my $current_line_size = 0;

my $pass = 0;
my @result = ();
my $last_char = '';
my $last_punctuation = '';
my $in_string = 0;
my $in_comment = 0;
my $singleline_comment = 0;
my $variables = {};
my $variable_prefix = '';
my $last_word = '';

sub debug
{
  if($debug)
  {
    printf { \*STDERR }  ("-> %s\n", $_[0]);
  }
}

sub append_to_result
{
  my $item = $_[0];
  $result[$pass] = $result[$pass] . $item;
  $current_line_size = $current_line_size + length($item);
  if($current_line_size >= $max_line_size)
  {
    $current_line_size = 0;
    $result[$pass] = $result[$pass] . "\n";
  }
}

sub remove_last
{
  $result[$pass] = substr($result[$pass], 0, -1);
}

sub is_keyword
{
  my $word = $_[0];
  foreach my $kw (@{$lang->{'keywords'}})
  {
    if($word =~ /$kw/)
    {
      return 1;
    }
  }
  return 0;
}

sub is_var_keyword
{
  my $word = $_[0];
  foreach my $kw (@{$lang->{'var_keywords'}})
  {
    if($word =~ /$kw/)
    {
      return 1;
    }
  }
  return 0;
}

sub in_list
{
  my ($list, $char) = @_[0..1];
  if($char ~~ @{$list}) { return 1; }
  return 0;
}

sub is_whitespace
{
  my $char = $_[0];
  return in_list($lang->{'space'}, $char);
}

sub is_punctuation
{
  my $char = $_[0];
  return in_list($lang->{'punctuation'}, $char);
}

sub is_delimiter
{
  my $char = $_[0];
  if(is_whitespace($char) or is_punctuation($char) or $char eq '') { return 1; }
  return 0;
}

sub is_digit
{
  my $char = $_[0];
  return in_list($lang->{'digit'}, $char);
}

sub is_string
{
  my $char = $_[0];
  if($char eq "\"" and not $in_string)
  {
    $in_string = 1;
    return 1;
  }
  if($char eq "\"" and $in_string and $last_char ne "\\")
  {
    $in_string = 0;
    return 0;
  }
  return $in_string;
}

sub is_comment
{
  my $char = $_[0];
  if($in_string) { return 0; }
  if($char eq "/" and $last_char eq "/")
  {
    remove_last();
    $in_comment = 1;
    $singleline_comment = 1;
    return 1;
  }
  if($char eq "*" and $last_char eq "/")
  {
    remove_last();
    $in_comment = 1;
    $singleline_comment = 0;
    return 1;
  }
  if($in_comment and $char eq "/" and $last_char eq "*" and $singleline_comment == 0)
  {
    $in_comment = 0;
    return 1;
  }
  if($in_comment and $char eq "\n" and $singleline_comment == 1)
  {
    $in_comment = 0;
    return 0;
  }
  return $in_comment;
}

sub short_name_4_var_exists
{
  my $var = $_[0];
  if(exists($variables->{$var})) { return 1; }
  return 0;
}

sub short_name_exists
{
  my $short_var = $_[0];
  foreach my $k (keys(%{$variables}))
  {
    if($variables->{$k} eq $short_var) { return 1; }
  }
  return 0;
}

sub get_short_name
{
  my $var = $_[0];
  return $variables->{$var};
}

sub generate_short_name
{
  my ($var, $prefix_i) = @_[0..1];
  my $prefix = $prefix_i >= 0 ? $alphabet[$prefix_i] : '';
  foreach my $i (0 .. $#alphabet)
  {
    my $symbol = $prefix . $alphabet[$i];
    if(not short_name_exists($symbol)) 
    { 
      $variables->{$var} = $symbol; 
      debug(sprintf("%s->%s", $symbol, $var));
      return $symbol;
    }
  }
  return generate_short_name($var, ++$prefix_i);
}

sub process_variable
{
  my $var = $_[0];
  if($in_string) { append_to_result($var); return; }
  if(is_digit(substr($var, 0, 1))) { append_to_result($var); return; }
  if(short_name_4_var_exists($var)) 
  { 
    append_to_result($pass == 1 ? get_short_name($var) : $var); 
    return; 
  }
  if(is_var_keyword($last_word)) 
  { 
    generate_short_name($var, -1);
    append_to_result($var);
    #append_to_result($pass == 1 ? generate_short_name($var, -1)); 
    return; 
  }
  append_to_result($var);
  #printf("!%s!", $var);
}

sub process_word
{
  my $word = $_[0];
  #debug($word);
  if(is_keyword($word))
  {
    debug(sprintf('[%s]', $word));
    append_to_result($word);
    return;
  }
  process_variable($word);
}

sub main
{
  my $src = $_[0];
  my $current_word = '';
  my $punctuation_inserted = 1;
  for my $char (split('', $src))
  {
    if(not is_comment($char))
    {
      if(is_delimiter($char))
      {
        if($current_word ne '')
        {
          unless($punctuation_inserted) { append_to_result(' '); }
          process_word($current_word);
          $last_word = $current_word;
          $current_word = '';
          $punctuation_inserted = 0;
          $last_punctuation = '';
        }
        if(is_string($char) and is_whitespace($char)) { append_to_result(' '); }
        if(is_punctuation($char))
        {
          debug(sprintf('{%s}', $char));
          append_to_result($char);
          $punctuation_inserted = 1;
          $last_punctuation = $char;
        }
      }
      else
      {
        $current_word = $current_word . $char;
      }
    }
    $last_char = $char;
  }
}

#debug(Dumper($lang));
#print(Dumper(@alphabet));
my $src = '';
for my $line (<>)
{
  chomp($line);
  $src .= $line;
}
main($src);
$pass++;
main($result[0]);
print($result[$pass]);
#debug(Dumper(@result));
print("\n");