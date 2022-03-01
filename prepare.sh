#!/bin/bash

project=$1
outfolder=$2
filelist=()

function collect_includes()
{
  src_file=$1
  for include in $(cat ${src_file} | grep '#include' | sed -r 's/^.*include (.*)$/\1/ig')
  do
    if [[ ! " ${filelist[@]} " =~ " ${include} " ]]
    then
      echo "Including ${include}..."
      filelist+=($include)
      collect_includes "${include}"
    fi
  done
}
collect_includes "${project}.cs"
cat ${filelist[@]} ${project}.cs | \
  sed -r 's@CBlocks@CB@g' | \
  sed -r 's@CBTyped@CBT@g' | \
  sed -r 's@CBBase@CBB@g' | \
  sed -r 's@CBlockGroup@CBG@g' | \
  sed -r 's@CBlockOptions@CBO@g' | \
  sed -r 's@CFunctional@CF@g' | \
  sed -r 's@options@o@g' | \
  sed -r 's@getValue@g@g' | \
  egrep -v "^ +?(//|$)" | \
  astyle --mode=cs --style=lisp --indent=force-tab --max-code-length=200 --unpad-paren --pad-comma \
         --keep-one-line-statements --keep-one-line-blocks | \
  sed -r 's@ +@ @g' > ${outfolder}/${project}.cs
