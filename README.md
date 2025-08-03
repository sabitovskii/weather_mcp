# Weather MCP Server

This is a test assignment for FastMCP.me

It provides real weather information using OpenWeatherMap API and exposes tools for AI agents like Claude or GitHub Copilot via the Model Context Protocol (MCP).

---

## ✅ Features

- `GetCurrentWeather` — fetches current weather for any location
- `GetWeatherForecast` — provides 3-day weather forecast
- Real-time data via OpenWeatherMap
- Full support for multiple cities / countries
- Proper error handling and logging
- Fully compatible with GitHub Copilot MCP integration

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [OpenWeatherMap API Key](https://home.openweathermap.org/users/sign_up)
- [GitHub Copilot Chat in VSCode](https://github.com/github/copilot)

---

### 1. Clone the repo

```bash
git clone https://github.com/sabitovskii/weather_mcp.git
cd weather_mcp
```

---

### 2. Add your API key (one of the options below)

#### Option A: via VSCode `.vscode/launch.json`

```json
"env": {
  "OPENWEATHER_API_KEY": "your_key_here"
}
```

#### Option B: via terminal (PowerShell)

```powershell
$env:OPENWEATHER_API_KEY="your_key_here"
dotnet run
```

---

### 3. Run the project

```bash
dotnet run
```

You should see console output. The server runs using `stdio` transport, ready to be consumed by Copilot or Claude.

---

### 4. Enable GitHub Copilot MCP integration

Create a file:

```
.vscode/mcp.json
```

With content:

```json
{
  "servers": {
    "SampleMcpServer": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "."
      ],
      "env": {
        "OPENWEATHER_API_KEY": "<your_api_value_here>"
      }
    }
  }
}
```

Restart VSCode. Now Copilot can use your tools directly in chat.

---

## 🛠 Tools Overview

### `GetCurrentWeather`

Gets current weather for a given city.

```json
{
  "tool": "GetCurrentWeather",
  "parameters": {
    "city": "Almaty",
    "countryCode": "KZ"
  }
}
```

---

### `GetWeatherForecast`

Returns 3-day forecast for a given location (in 3-hour intervals).

```json
{
  "tool": "GetWeatherForecast",
  "parameters": {
    "city": "Berlin"
  }
}
```
