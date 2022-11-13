# Team Panda


## Git Asset Module
Das Level-Design Projekt https://git.informatik.fh-nuernberg.de/team-panda/level-design ist über ein Submodul eingebunden. Damit das Projekt automatisch aktualisiert wird, muss die git config angepasst werden:
```bash
$ git submodule update --init --recursive
```

```bash
$ git submodule update --remote --recursive
```
