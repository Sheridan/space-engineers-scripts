OUT_PATH=prepared

clean:
	rm -f ${OUT_PATH}/*

prepare:
	./prepare.sh ${project} ${OUT_PATH}

to_clipboard:
	cat ${OUT_PATH}/${project}.cs | xclip -selection clipboard -i

setup: project=setup
setup: prepare to_clipboard

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

mole_digger_auto_horizont: project=mole_digger_auto_horizont
mole_digger_auto_horizont: prepare to_clipboard

ship_auto_horizont: project=ship_auto_horizont
ship_auto_horizont: prepare to_clipboard

components_planning: project=components_planning
components_planning: prepare to_clipboard
