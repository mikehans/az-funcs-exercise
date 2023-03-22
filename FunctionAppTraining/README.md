# Azure Functions exercises
## What's this?
It's a set of 4 Azure Functions that obtains, stores and allows retrieval of posts data from https://jsonplaceholder.typicode.com/posts.

The test data returns 100 fake posts.

## Functions
* GetPostsData
  * HTTP trigger
  * gets data and writes file to blob storage
* ParsePostsData
  * Blob trigger
  * gets data from blob storage
  * parses JSON
  * writes each post to queue storage
* ParseQueueToCosmosDb
  * Queue storage trigger
  * gets items from queue storage and writes to Cosmos DB
* GetPostByPostId
  * HTTP trigger
  * takes a Post ID and queries CosmosDB for it, returning data

## Setup
### Run locally
* ensure Azurite and CosmosDB emulator are installed and running
* Create local.settings.json
  * fill details for ```CosmosDbConnectionString``` and ```AzureWebJobsStorage``` from the emulators
    ```json
        {
          "IsEncrypted": false,
          "Values": {
            "CosmosDbConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=whatever",
            "AzureWebJobsStorage": "AccountName=devstoreaccount1;AccountKey=whatever;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;",
            "FUNCTIONS_WORKER_RUNTIME": "dotnet",
            "FUNCTIONS_EXTENSION_VERSION": "~4"
          }
        }
    ```

## Notes

