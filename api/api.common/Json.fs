module Djambi.Api.Common.Json

open System.Text.Json
open System.Text.Json.Serialization

let options =
    let options = JsonFSharpOptions.Default().ToJsonSerializerOptions()
    options.Converters.Add(JsonStringEnumConverter())
    options

let serialize<'a> (source: 'a) : string =
    JsonSerializer.Serialize(source, options)

let deserialize<'a> (source: string) : 'a =
    JsonSerializer.Deserialize<'a>(source, options)
