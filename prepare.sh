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

function sed_preproc()
{
  cat ${filelist[@]} ${project}.cs | \
   replace "CBlocks"                  "CB"   | \
   replace "CBTyped"                  "CBT"  | \
   replace "CBlockGroup"              "CBG"  | \
   replace "CBBase"                   "CBB"  | \
   replace "CBlockOptions"            "CBO"  | \
   replace "CFunctional"              "CF"   | \
   replace "options"                  "o"    | \
   replace "getValue"                 "g"    | \
   replace "toHumanReadable"          "tHR"  | \
   replace "previousIsWhitespace"     "pIW"  | \
   replace "setup"                    "s"    | \
   replace "CDisplay"                 "CD"   | \
   replace "CConnector"               "CC"   | \
   replace "CLandingGear"             "CLG"  | \
   replace "CBattery"                 "CBt"  | \
   replace "CBlockStatusDisplay"      "CBSD" | \
   replace "CBlockPowerInfo"          "CBPI" | \
   replace "CBlockUpgrades"           "CBU"  | \
   replace "CTextSurface"             "CTS"  | \
   replace "CTerminal"                "CT"   | \
   replace "countSurfacesX"           "csX"  | \
   replace "countSurfacesY"           "csY"  | \
   replace "m_surfaces"               "m_s"  | \
   replace "result"                   "r"    | \
   replace "self"                     "_"    | \
   replace "showStatus"               "sS"   | \
   replace "addDisplay"               "aD"   | \
   replace "EHRUnit"                  "EHRU" | \
   replace "getPistonsStatus"         "gPsS" | \
   replace "getConnectorsStatus"      "gCS"  | \
   replace "getMergersStatus"         "gMS"  | \
   replace "getProjectorsStatus"      "gPS"  | \
   replace "getRotorsStatus"          "gRS"  | \
   replace "getGyroStatus"            "gGS"  | \
   replace "getBatteryesStatus"       "gBS"  | \
   replace "getGasTanksStatus"        "gGTS" | \
   replace "getPowerProducersStatus"  "gPPS" | \
   replace "getInvertoryesStatus"     "gIS"  | \
   replace "getFunctionaBlocksStatus" "gFBS" | \
   replace "CRecipe"                  "cR"   | \
   replace "FComponentItem"           "FCI"  | \
   replace "CComponentItem"           "CCI"  | \
   replace "EItemType"                "EIT"  | \
   replace "isAssignable"             "iA"   | \
   replace "structureName"            "sN"   | \
   replace "CSetup"                   "CS"   | \
   replace "FRecipe"                  "FR"   | \
   replace "recipe"                   "R"    | \
   replace "amount"                   "a"    | \
   replace "//.*$"                ""     | \
   egrep -v "^ +?(//|$)" | \
   astyle --mode=cs --style=lisp --indent=spaces --max-code-length=200 --unpad-paren --pad-comma \
          --keep-one-line-statements --keep-one-line-blocks --convert-tabs --delete-empty-lines | \
   sed -r 's@^ +@@g' | \
   sed -r 's@ +@ @g'   \
     > ${outfolder}/${project}.cs
}

function reduce_preproc()
{
  cat ${filelist[@]} ${project}.cs | \
     replace "//.*$"                ""     | \
    ./reduce.pl \
    > ${outfolder}/${project}.cs
}

function calc_chars()
{
  wc -c ${outfolder}/${project}.cs
}

calc_chars
collect_includes "${project}.cs"
sed_preproc
#reduce_preproc
calc_chars

# clang-format --style=file --verbose
#  astyle --mode=cs --style=lisp --indent=spaces --max-code-length=200 --unpad-paren --pad-comma \
#         --keep-one-line-statements --keep-one-line-blocks --convert-tabs --delete-empty-lines
#  sed -r 's@visibleIn@vI@g' | \
#   sed -r 's@[^"].* +?([[:punct:]]) +?.*[^"]@\1@g' | \
