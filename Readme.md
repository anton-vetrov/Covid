The following assumptions were made during the implementation:
1. Either county, or state name is supposed to be provided
2. Hence, service provides six end-points: three for state and three for county
3. Service uses Web API to project CSV-file hosted in GitHub - https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv.
4. The file is a part of https://github.com/CSSEGISandData/COVID-19/tree/master/csse_covid_19_data/csse_covid_19_time_series project supposed to be uploaded daily. 
5. Paging is introduced for large sets of data
6. Project is continuously deployed in Azure test environment at https://app-av-covidservice-test.azurewebsites.net/swagger/index.html

How to build the project:
1. Optionally update DockerfileRunArguments variable in launchSettings.json if you are launching using Docker profile
2. Open Visual Studio 2022 and hit build all solution

How to run the project locally:
1. Set CovidService as an active project and Start without a debugging(Ctrl-F5)
2. Swagger document is available at http://localhost:5000/swagger/index.html
3. Postman collection is included in Covid\CovidService.postman_collection.json
4. Import Postman collection into Postman and use collection to test the service out

How to a access the project deployed in Azure:
1. Open swagger URL https://app-av-covidservice-test.azurewebsites.net/swagger/index.html
2. Use swagger, or Postman collection included with the project to run APIs
3. To use Postman, you may need to set {{serverUrl}} variable to https://app-av-covidservice-test in the corresponding PostMan environment 