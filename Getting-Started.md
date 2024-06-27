## Get watching
```
dotnet watch --project ./api
```

## Get testing

dotnet tool install -g dotnet-reportgenerator-globaltool

```
cd apiTests
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults --logger "console;verbosity=normal"
reportgenerator "-reports:./TestResults/**/coverage.cobertura.xml" "-targetdir:./../api/wwwroot/test" "-reporttypes:Html"
```


## Get angling
```
cd stargate-ui
ng completion
# source <(ng completion script)  # seems slow?
ng build --watch --configuration production --verbose
```
Todo: 
- configure angular app to read settings?

## settings.json file for preview browser
```markdown
[settings.json](.vscode/settings.json)
```
