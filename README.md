# ODataBenchmark
OData benchmark for .net 5 using EF memory database

Benchmark compares certain query performance with matching api calls
To reduce hardware dependency in memory database is used

Following result achieved on AMD Ryzen 4750U laptop
```json
[
  {
    "Name": "All Customers, level 1 expand, small size collection",
    "Types": [
      {
        "TestType": "Odata",
        "AvgDurationMs": 92.261,
        "AvgPayloadSizeBytes": 60714
      },
      {
        "TestType": "Api",
        "AvgDurationMs": 34.418,
        "AvgPayloadSizeBytes": 60378
      }
    ]
  },
  {
    "Name": "All Projects, level 2 expands, average size collection",
    "Types": [
      {
        "TestType": "Odata",
        "AvgDurationMs": 654.435,
        "AvgPayloadSizeBytes": 529087
      },
      {
        "TestType": "Api",
        "AvgDurationMs": 326.964,
        "AvgPayloadSizeBytes": 431227
      }
    ]
  },
  {
    "Name": "All Scopes, no expands, average size collection",
    "Types": [
      {
        "TestType": "Odata",
        "AvgDurationMs": 72.764,
        "AvgPayloadSizeBytes": 395596
      },
      {
        "TestType": "Api",
        "AvgDurationMs": 5.13,
        "AvgPayloadSizeBytes": 395521
      }
    ]
  },
  {
    "Name": "All Work Items, no expands, huge size collection",
    "Types": [
      {
        "TestType": "Api",
        "AvgDurationMs": 17.451,
        "AvgPayloadSizeBytes": 927883
      },
      {
        "TestType": "Odata",
        "AvgDurationMs": 400.581,
        "AvgPayloadSizeBytes": 1032287
      }
    ]
  },
  {
    "Name": "Single Project by Id, level 2 expands",
    "Types": [
      {
        "TestType": "Odata",
        "AvgDurationMs": 5.786,
        "AvgPayloadSizeBytes": 2198.716
      },
      {
        "TestType": "Api",
        "AvgDurationMs": 3.705,
        "AvgPayloadSizeBytes": 1706.345
      }
    ]
  },
  {
    "Name": "Single Project by Id, level 3 expands",
    "Types": [
      {
        "TestType": "Api",
        "AvgDurationMs": 3.479,
        "AvgPayloadSizeBytes": 8213.06
      },
      {
        "TestType": "Odata",
        "AvgDurationMs": 12.034,
        "AvgPayloadSizeBytes": 11434.536
      }
    ]
  }
]
```