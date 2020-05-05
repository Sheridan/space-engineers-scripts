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
  egrep -v "^ +?(//|$)" | \
  astyle --mode=cs --style=lisp --indent=force-tab --max-code-length=200 --unpad-paren --pad-comma \
         --keep-one-line-statements --keep-one-line-blocks | \
  sed -r 's@ +@ @g' > ${outfolder}/${project}.cs
