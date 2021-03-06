AutoZilla
=========
AutoZilla is a .Net alternative to AutoHotKey/AutoIt. It consists of a core
library which provides: 

- A way of registering global hot keys
- A template system
- A way of outputting text to applications
- A basis for writing and loading plugins

A main application runs as a task-tray application and allows the user to load
and unload plugins and see which HotKeys they have registered.


Solution Contents
=================
**AutoZilla** is the main GUI.

**AutoZilla.Core** is the core library which you need to reference to
create a plugin.

**AutoZilla.Core.Tests** is a set of unit tests for AutoZilla.Core.

**AutoZilla.Demo1** is a very simple, easy to understand demo project
which shows the minimum needed to register a HotKey and run a callback.

**AutoZilla.Plugins.MSSQL** is a full-featured plugin for auto-typing MS SQL
related text.

**ConfigZilla** this is my nuget package for automatically generating
.config files based on MSBuild settings.

**MiscUtils** a git subtree linked to my MiscUtils repository.


A note on Building
==================
Building is non-standard in this project.

The "Config" project is used to transform *.template.config
files into *.config files. See my blogpost at XXX for how this is done.


Projects Used
=============
IniParser : https://gist.github.com/grumly57/5725301
GlobalHotKeys : https://github.com/curtisrutland/LowLevelHooks


TODO
====
- [ ] Templates: lots more built-in variables, see the issue in Github.
- [ ] Templates: How to have a button on a form do the same thing as running a template?
- [ ] Templates: Create an "AUTOZILLA.ALL" variable for documentation purposes.
- [ ] Templates: allow templates in templates without needing custom code.
- [ ] Tooling: setup FxCop for the project.
- [ ] Main GUI: Pick one HotKey to display the form?
- [ ] Main GUI: get decent icons in 16x16 for the tray, larger for the toolbar
- [ ] Main GUI: create a log4net appender for the debug tab
- [ ] Main GUI: provide ability to load & unload keys & plugins, and block particular ones.
- [ ] Main GUI: provide a setting to hide the Debug tab.
- [ ] Demos: Create a demo (simplest possible) plugin?
- [ ] Demos: Create a demo WPF project?
- [ ] Core: Create a FocusRestoringFormBase
- [ ] Core: How do we handle versions - 0.1 etc.
- [ ] Config & Build: figure out how to make the build process copy a template if it has changed.
- [ ] Plugins/AutoTemplates: Wikipedia tables! and continued lists
- [X] TemplateLoader class: load from known "Templates" subfolder. DONE.
- [X] Templates: "emplatize" refactor. REMOVED.
- [X] Templates: replace variables via an event? DONE.
- [X] ModifiedKey: the VK issue - how to translate CA-; ? SOLVED! ; is Oem1 in the UK...
- [X] Config: only generate Transform.xsl once. DONE.
- [X] Vi: need to send a prefix? Doesn't work with paste either. N/A.
      Don't bother, wouldn't work anyway because we would never know which mode we were in.
