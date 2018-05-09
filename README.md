# REST API Best Practices

This is a quick start guide to developing REST APIs with dotnet core

---

## dotnet core

In dotnet core MVC 6 there is no separation from MVC Controllers and Web API Controllers. These are now the same Controller implementation but you can return different responses depending on the Controller Action.

---

## Naming

### RESTFul Endpoints

#### Nouns over Verbs

Use Nouns over Verbs for RESTFul endpoints
```
GET /stations
GET /stations/123
POST /stations
```

#### Entity Per Controller

> TODO

## HTTP Methods

| HTTP Method	   | When to use?					|
| ---			   | ---							|
| GET			   | Returning a Resource / Data		|
| POST			   | Adding new Resource / Complex Searches / Form Posts |
| PUT			   | Updating a Resource on the Server		|
| DELETE		   | Returning Entity / Data		|

## Common HTTP Status Codes
* **1xx** Information
* **2xx** Success
* **3xx** Redirection
* **4xx** Client Error
* **5xx** Server Error


| HTTP Status Code | Friendly Name		| When to use? |
| :--------------: | :-----------		| :---------- |
| **200**			   | OK					| When a request was Success	|
| **201**			   | Created			| When a Resource has successfull been created on the server	|
| **204**			   | No Content			| When a Resource has had an action peformed but no content to return in response. I.e. PUT or DELETE with no return response will return 204.	|
| **301**			   | Moved Permanently	| When a Resource has been moved permanently to another location	|
| **307**			   | Moved Temporarily	| When a Resource has been moved temporarily to another location	|
| **400**			   | Bad Request		| When there has been a handled issue on the server which is invalid. For example bad request parameters |
| **401**			   | Unauthorized		| When request for a resource has been denied due to authorization|
| **403**			   | Forbidden			| When a request has been received but server will not fulfil it |
| **404**			   | Not Found			| When the Resource has not been found|
| **500**			   | Internal Server Error	| When an unexpected error has occurred on the server |
| **501**			   | Not Implemented	| |
| **502**			   | Bad Gateway		| |
| **503**			   | Service Unavailable| |
| **504**			   | Gateway Timeout	| |


&nbsp;
&nbsp;

## Content Negotiation
### Accept: application/json

The client requests via HTTP Header to request resource in particular format. For APIs this is most common as JSON but could be XML.


### Content-Type: application/json

The client request body format sent to the server. In APIs this is again common to be JSON.

---

## HATEOS

The HATEOAS approach enables a client to navigate and discover resources from an initial starting point. This is achieved by using links containing URIs; when a client issues an HTTP GET request to obtain a resource, the response should contain URIs that enable a client application to quickly locate any directly related resources. 

### Paging in HATEOS

See example in this repo <https://github.com/nbarbettini/BeautifulRestApi/blob/master/src/Models/CollectionWithPaging%7BT%7D.cs>

```json
{
    "href": "http://api.foo.bar/comments",
    "rel": [ "collection" ],
    "offset": 0,
    "limit": 25,
    "size": 200,
    "first": { "href": "http://api.foo.bar/comments", "rel": [ "collection" ] },
    "next": { "href": "http://api.foo.bar/comments?limit=25&offset=25", "rel": [ "collection" ] },
    "last": { "href": "http://api.foo.bar/comments?limit=25&offset=175", "rel": [ "collection"] },
    "value": [
      "items..."
    ]
}
```

> Note this needs to be defined more with examples

## Client-side Caching



> Note this needs to be defined

--- 

# API Documentation

It is essential that APIs have self documenting output for both internal and external developers. 
This also is extremely important to ensure the APIs are thought out and thinking of the usage of the API.


## Swagger implemented using Swashbuckle

&nbsp;
---


# Security

## dotnet core Claim Based Policies

```csharp
[Authorize(Policy = "Admin")]
public IActionResult GetSecuredResource()
{
    return View();
}

```

&nbsp;
---


# API Models

## Request / Response DTOs

It is important to separate both Requests, Responses for the Request which the API methods.

> PersonAdd is different to PersonEdit & have different validation rules.

> CQRS is a great architectual approach to separting out APIs for Command & Queries in APIs
  
&nbsp;
---

# Controllers

* Use return type **IActionResult**
* Always use **async Task**

```csharp
public IActionResult<Task<object>> GetAll() {
	// Return data
}
```

## Asynchronous APIs

The dotnet core framework supports async down the full pipeline from the Request to the Data with Entity Framework Core.
Use async always for peformance. This means you need to implement await in your methods when returning async

> Adding a **CancellationToken** parameter to your route methods allows ASP.NET Core to notify your asynchronous tasks of a cancellation (if the browser closes a connection, for example).


### async / await

> TODO

## Routing

> TODO

## Route Validation

> TODO

## Model Validation

> TODO - Possible ActionFilter for Model Validation

## Model Binding

> TODO

&nbsp;
---


# Error Handling

* Capture exceptions and return ErrorModel
* Return Model Errors only when Authorized Request
  * You do not want to leak error messages to unknown users which could use information for malicious requests
* Global Error Handling for Internal Server Errors (500s)
* Return appropiate Status Code for API Request

--- 

# API Testing

## Controllers
Controller Tests should be lightweight as this layer is about concerns of calling services, handling requests and returning correct responses.

**Controller Tests**
* Method Input Parameters
* Dependency Calls
* Model Binding
* Content Negotiation
* HTTP Status Codes
* Response Models
* Paging

### Example
```csharp
        [Fact]
        public async Task StationsController_GetAll_ReturnsOKAndStations()
        {
            // Arrange
            var controller = new StationsController(new StationService());

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var stations = okResult.Value.Should().BeAssignableTo<IEnumerable<Station>>().Subject;

            stations.Count().Should().Be(50);
        }
```

## Services

Services should contain the business logic so that they are reusable and concerns are in Services not in the Controllers.

* Business Logic
* Assertions
* Validation

## API Integration Tests (dotnet core project)

Testing dotnet core API projects using dotnet core TestServer which allows Integrations Tests a breeze.
This will test the Middleware & Startup of the API application.
The same Integration Tests are required to deal with Response from HttpClient.

```csharp

        [Fact]
        public async Task Stations_Get_All()
        {
            // Arrange
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
             var client = _server.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.GetAsync("/api/Stations");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var stations = JsonConvert.DeserializeObject<IEnumerable<Station>>(responseString);
            stations.Count().Should().Be(50);
        }

```

## API Integration Tests

Testing local or server API endpoints using HTTP request & responses. This is proving the APIs in real usage testing the full end to end pipeline in the application.

### Example
```csharp
        [Fact]
        public async Task GetStations_WhenHitNetworkAndBySydneyPostcode_ThenReturnsStatusOK()
        {
            // Arrange
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://referenceapi-test.scalabs.com.au");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.GetAsync("/api/v1/hit/stations?IsNearestSearch=true&Postcode=2000");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

			// Validate Models

            responseString.Should().NotBeNullOrEmpty();
        }
```