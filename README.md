# ODataBenchmark
OData benchmark for .net 5 using EF memory database

Benchmark compares certain query performance with matching api calls
To reduce hardware dependency in memory database is used

Following result achieved on AMD Ryzen 4750U laptop

    ]
  },
  {
    "Name": "All Projects, level 2 expands, average size collection",
    "Types": [
      {
        "TestType": "Api",
        "AvgDurationMs": 317.373,
        "AvgPayloadSizeBytes": 446522
      },
      {
        "TestType": "Odata",
        "AvgDurationMs": 639.7224448897796,
        "AvgPayloadSizeBytes": 539480.126252505
      }
    ]
  },
  {
    "Name": "Single Project by Id, level 3 expands",
    "Types": [
      {
        "TestType": "Api",
        "AvgDurationMs": 3.652,
        "AvgPayloadSizeBytes": 8369.304
      },
      {
        "TestType": "Odata",
        "AvgDurationMs": 12.63152610441767,
        "AvgPayloadSizeBytes": 11571.939759036144
      }
    ]
  },
  {
    "Name": "All Scopes, no expands, average size collection",
    "Types": [
      {
        "TestType": "Odata",
        "AvgDurationMs": 71.48995983935743,
        "AvgPayloadSizeBytes": 395692.8835341365
      },
      {
        "TestType": "Api",
        "AvgDurationMs": 5.226,
        "AvgPayloadSizeBytes": 395221
      }
    ]
  }
]
