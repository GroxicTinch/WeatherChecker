# Weather Checker

> **Important:** Before running this project, make sure you add your geocoding API key.
> You can set it as an environment variable ("GEOCODING_API_KEY") or if nessessary in your `appsettings.Development.json` file (setting inside the file is purely for development, do **NOT** use in production).

## Description

Weather Checker is a simple web application built with **Vue.js** (frontend) and **C# .NET** (backend) that allows users to search for cities or other locations and view weather data including temperatures, precipitation, UV index, and more.

The app fetches:
- Geocoding data (latitude/longitude) from **[geocode.maps.co](https://geocode.maps.co)**
- Weather data from **[Open-Meteo API](https://open-meteo.com/en/docs)**

## Features

- Search for cities with auto-complete suggestions.
- View daily and current weather.
- Save favorite locations locally(browser storage).
