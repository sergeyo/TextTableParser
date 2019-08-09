# TextTableParser

[![Build status](https://ci.appveyor.com/api/projects/status/5nlje16xv1ab669l?svg=true)](https://ci.appveyor.com/project/sergeyo/texttableparser)
[![nuget][nuget-badge]][nuget-url]

[nuget-badge]: https://img.shields.io/badge/nuget-v0.1.0-blue.svg
[nuget-url]: https://www.nuget.org/packages/TextTableParser


A set of classes for text files to table parsing and for a text table convertion to DTO of your type.

It has 2 main classes: the CSVParser class - to parse text csv/tsv files to Table class, and the ITableToDtoConverter interface, which can convert an object of the Table class to the collection of your Dtos.

<pre>
    var table = new CSVParser(true, '\t', null).CreateFromCsv("file.tsv");

    var converter = new TableToDtoConverterFactory().GetParser&lt;TestDto&gt;();

    IEnumerable&lt;TestDto&gt; dtos = converter.Convert(table);
</pre>
