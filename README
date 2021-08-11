# POC - Wiremock Stub Mapping Recording

Provides a example/proof of concept of Wiremock Stub Mapping Recording using .NET Core.


## The Project

This project contains 3 elements:
- The API (Wiremock-Recording.Server.Api): which contains a simple API with two endpoints;
- Recording Test (Wiremock-Recording.Test): A NUnit test project which test and record the response from the API;
- Playback Test (Wiremock-Playback.Test): Another NUnit test project which tests the APIs endpoint but using only the Wiremock Mappings - WITHOUT running the aforementioned API;

## How to run

The points of interest of this project are the test projects. In order to get it working, you have to run FIRST the _Recording Test_ and then _Playback Test_ at least once (in order to create the stubs).
