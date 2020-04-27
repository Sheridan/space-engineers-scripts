OUT_PATH=prepared

clean:
	rm -f ${OUT_PATH}/*

prepare:
	./prepare.sh ${project} ${OUT_PATH}

to_clipboard:
	cat ${OUT_PATH}/${project}.cs | xclip -selection clipboard -i

mole_builder: project=mole_builder
mole_builder: prepare to_clipboard

mole_builder_status_display: project=mole_builder
mole_builder_status_display: prepare to_clipboard

mole_digger: project=mole_digger
mole_digger: prepare to_clipboard

mole_digger_status_display: project=mole_digger_status_display
mole_digger_status_display: prepare to_clipboard

display_test: project=display_test
display_test: prepare to_clipboard

auto_horizont: project=auto_horizont
auto_horizont: prepare to_clipboard
