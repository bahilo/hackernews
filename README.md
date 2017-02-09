# Web scraping application

This console application has been developed in the .NET enrironment using C# language.
The application starts the scraping by downloading the web page, then through XPATH patterns extracts the differents pieces of information.

## Usage

```
> cd hackernews/bin/Debug
> hackernews.exe --posts n
---------------------------------------
n = Number of posts requested ( less or equals to 100)
```

## Example

```
> hackernews.exe --posts 3
[
  {
    "Title": "US Army approves Dakota Access Pipeline without required environmental review",
    "Uri": "https://www.google.com/amp/www.bbc.co.uk/news/amp/38901498",
    "Author": "socialentp",
    "Points": 156,
    "Comments": 110,
    "Rank": 1
  },
  {
    "Title": "The most mentioned books on Stack Overflow",
    "Uri": "http://www.dev-books.com",
    "Author": "vladwetzel",
    "Points": 290,
    "Comments": 129,
    "Rank": 2
  },
  {
    "Title": "Do People View All 360°?",
    "Uri": "https://blog.vrtigo.io/do-people-view-all-360-f60b858059fe#.g9ghlbjl4",
    "Author": "mdchaudhari",
    "Points": 56,
    "Comments": 29,
    "Rank": 3
  }
]
```

## Libraries

- `Newtonsoft.Json` this library allows serialising .Net object to JSON. It is really easy to use.
- `HtmlAgilityPack` is a library to parse html page using xpath and XSLT. I used this library for the fact that the object model is really similar to System.Xml
