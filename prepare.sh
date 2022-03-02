#!/bin/bash

project=$1
outfolder=$2
filelist=()

function collect_includes()
{
  src_file=$1
  for include in $(cat ${src_file} | grep '#include' | sed -r 's/^.*include (.*)$/\1/ig') #'
  do
    if [[ ! " ${filelist[@]} " =~ " ${include} " ]]
    then
      echo "Including ${include}..."
      filelist+=($include)
      collect_includes "${include}"
    fi
  done
}

function replace()
{
  regexp=$1
  what=$2
  sed -r "s@${regexp}@${what}@g"
}

collect_includes "${project}.cs"
cat ${filelist[@]} ${project}.cs | \
  replace "CBlocks"              "CB"   | \
  replace "CBTyped"              "CBT"  | \
  replace "CBlockGroup"          "CBG"  | \
  replace "CBBase"               "CBB"  | \
  replace "CBlockOptions"        "CBO"  | \
  replace "CFunctional"          "CF"   | \
  replace "options"              "o"    | \
  replace "getValue"             "g"    | \
  replace "toHumanReadable"      "tHR"  | \
  replace "previousIsWhitespace" "pIW"  | \
  replace "setup"                "s"    | \
  replace "CDisplay"             "CD"   | \
  replace "CBlockStatusDisplay"  "CBSD" | \
  replace "CBlockPowerInfo"      "CBPI" | \
  replace "CBlockUpgrades"       "CBU"  | \
  replace "CTextSurface"         "CTS"  | \
  replace "CTerminal"            "CT"   | \
  replace "countSurfacesX"       "csX"  | \
  replace "countSurfacesY"       "csY"  | \
  replace "m_surfaces"           "m_s"  | \
  replace "showStatus"           "sS"   | \
  replace "addDisplay"           "aD"   | \
  replace "EHRUnit"              "EHRU" | \
  replace "//.*$"                ""     | \
  egrep -v "^ +?(//|$)" | \
  astyle --mode=cs --style=lisp --indent=spaces --max-code-length=200 --unpad-paren --pad-comma \
         --keep-one-line-statements --keep-one-line-blocks --convert-tabs --delete-empty-lines | \
  sed -r 's@^ +@@g' | \
  sed -r 's@ +@ @g'   \
    > ${outfolder}/${project}.cs

  wc -c ${outfolder}/${project}.cs

# clang-format --style=file --verbose
#  astyle --mode=cs --style=lisp --indent=spaces --max-code-length=200 --unpad-paren --pad-comma \
#         --keep-one-line-statements --keep-one-line-blocks --convert-tabs --delete-empty-lines
#  sed -r 's@visibleIn@vI@g' | \
#   sed -r 's@[^"].* +?([[:punct:]]) +?.*[^"]@\1@g' | \
