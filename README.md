"# Feed Caching Assignment" 

Understanding the Caching Architecture
Before we implement the code, let's visualize how introducing a caching layer completely changes the lifecycle of a request to your GET /feed/{userId} endpoint.

When a request comes in, the application will first check the cache. If the data is there (Cache Hit), it returns instantly. If it isn't (Cache Miss), it falls back to the database, populates the cache for next time, and returns the data.

You can interactively simulate this workflow below to see how Cache Hits, Cache Misses, and Time-To-Live (TTL) expiration directly impact your application's average response time.



