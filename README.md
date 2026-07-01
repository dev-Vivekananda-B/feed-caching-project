"# Feed Caching Assignment" 

Understanding the Caching Architecture
Before we implement the code, let's visualize how introducing a caching layer completely changes the lifecycle of a request to your GET /feed/{userId} endpoint.

When a request comes in, the application will first check the cache. If the data is there (Cache Hit), it returns instantly. If it isn't (Cache Miss), it falls back to the database, populates the cache for next time, and returns the data.

You can interactively simulate this workflow below to see how Cache Hits, Cache Misses, and Time-To-Live (TTL) expiration directly impact your application's average response time.


When the application is running. i.e. dotnet run 
<img width="1598" height="355" alt="image" src="https://github.com/user-attachments/assets/bf689054-0b3e-4f96-9d48-0a0cd48f70f1" />

when the users hit the Endpoint i.e.curl http://localhost:5266/feed/user123  we get the JSON with the lowest latecy(fast)

<img width="1701" height="284" alt="image" src="https://github.com/user-attachments/assets/e3b396d5-b021-4ec9-8030-3b63c3b79770" />

Cache hit and cache miss cases. 

<img width="1724" height="498" alt="image" src="https://github.com/user-attachments/assets/3b9e8e6f-7dd7-400d-80de-702d035b091a" />

