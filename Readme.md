During the implememtation the following assumptions were made:
1. Either county, or state name is supposed to be provided
2. Hence, service provides six end points: three for state and three for county
3. Service downloads CSV file from github url for the first time only. You need to re-run the service to download a new file 

How to build the project:
1. Open Visual Studio 2022 and hit build all the solution

How run the project:
1. Set CovidService as active project and Start without a debugging(Ctrl-F5)
2. Swagger document is available at http://localhost:5000/swagger/index.html
3. Postman collection is included in Covid\CovidService.postman_collection.json
4. Import Postman collection into Postman and use calls to test the service out