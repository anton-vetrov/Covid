The following assumptions were made during the implementation:
1. Either county, or state name is supposed to be provided
2. Hence, service provides six end-points: three for state and three for county
3. Service downloads CSV file from github url for the first time only. You need to re-run the service to download a new file
4. Paging is introduced for large sets of data

How to build the project:
1. Optionally update DockerfileRunArguments variable in launchSettings.json if you are launching using Docker profile
2. Open Visual Studio 2022 and hit build all the solution

How to run the project locally:
1. Set CovidService as an active project and Start without a debugging(Ctrl-F5)
2. Swagger document is available at http://localhost:5000/swagger/index.html
3. Postman collection is included in Covid\CovidService.postman_collection.json
4. Import Postman collection into Postman and use calls to test the service out

How to run the project deployed in Azure:
1. Open swagger URL https://app-av-covidservice-test.azurewebsites.net/swagger/index.html
2. Use swagger, or Postman collection included with the project to run APIs