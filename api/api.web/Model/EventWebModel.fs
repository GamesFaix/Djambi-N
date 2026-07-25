namespace Djambi.Api.Web.Model

open System
open System.ComponentModel.DataAnnotations
open System.Text.Json.Serialization
open Djambi.Api.Enums

[<JsonDerivedType(typeof<ChangeEffectDto<int list>>)>]
[<JsonDerivedType(typeof<ChangeEffectDto<GameStatus>>)>]
[<JsonDerivedType(typeof<ChangeEffectDto<PieceDto>>)>]
[<JsonDerivedType(typeof<ChangeEffectDto<PlayerStatus>>)>]
[<JsonDerivedType(typeof<ChangeEffectDto<TurnDto>>)>]
[<JsonDerivedType(typeof<PieceUpdateEffectDto>)>]
[<JsonDerivedType(typeof<NeutralPlayerAddedEffectDto>)>]
[<JsonDerivedType(typeof<PieceEnlistedEffectDto>)>]
[<JsonDerivedType(typeof<PieceMovedEffectDto>)>]
[<JsonDerivedType(typeof<PieceVacatedEffectDto>)>]
[<JsonDerivedType(typeof<PlayerAddedEffectDto>)>]
[<JsonDerivedType(typeof<PlayerOutOfMovesEffectDto>)>]
[<JsonDerivedType(typeof<PlayerRemovedEffectDto>)>]
[<JsonDerivedType(typeof<PlayerStatusChangedEffectDto>)>]
[<JsonDerivedType(typeof<TurnCyclePlayerFellFromPowerEffectDto>)>]
[<JsonDerivedType(typeof<TurnCyclePlayerRemovedEffectDto>)>]
[<JsonDerivedType(typeof<TurnCyclePlayerRoseToPowerEffectDto>)>]
[<AbstractClass>]
type EffectDto(kind : EffectKind) =
    [<Required>]
    member __.Kind = kind

and ChangeEffectDto<'a>(kind : EffectKind,
                        oldValue : 'a,
                        newValue : 'a) =
    inherit EffectDto(kind)
    [<Required>]
    member __.OldValue = oldValue
    [<Required>]
    member __.NewValue = newValue

and PieceUpdateEffectDto(kind : EffectKind, oldPiece : PieceDto) =
    inherit EffectDto(kind)
    [<Required>]
    member __.OldPiece = oldPiece

and NeutralPlayerAddedEffectDto(name : string, placeholderPlayerId : int) =
    inherit EffectDto(EffectKind.NeutralPlayerAdded)
    [<Required>]
    member __.Name = name
    [<Required>]
    member __.PlaceholderPlayerId = placeholderPlayerId

and PieceEnlistedEffectDto(oldPiece : PieceDto, newPlayerId : int) =
    inherit PieceUpdateEffectDto(EffectKind.PieceEnlisted, oldPiece)
    [<Required>]
    member __.NewPlayerId = newPlayerId

and PieceMovedEffectDto(oldPiece : PieceDto, newCellId : int) =
    inherit PieceUpdateEffectDto(EffectKind.PieceMoved, oldPiece)
    [<Required>]
    member __.NewCellId = newCellId

and PieceVacatedEffectDto(oldPiece : PieceDto, newCellId : int) =
    inherit PieceUpdateEffectDto(EffectKind.PieceVacated, oldPiece)
    [<Required>]
    member __.NewCellId = newCellId

and PlayerAddedEffectDto(name : string, userId : int, playerKind : PlayerKind) =
    inherit EffectDto(EffectKind.PlayerAdded)
    [<Required>]
    member __.Name = name
    [<Required>]
    member __.UserId = userId
    [<Required>]
    member __.PlayerKind = playerKind

and PlayerOutOfMovesEffectDto(playerId : int) =
    inherit EffectDto(EffectKind.PlayerOutOfMoves)
    [<Required>]
    member __.PlayerId = playerId
    
and PlayerRemovedEffectDto(oldPlayer : PlayerDto) =
    inherit EffectDto(EffectKind.PlayerRemoved)
    [<Required>]
    member __.OldPlayer = oldPlayer

and PlayerStatusChangedEffectDto(oldValue : PlayerStatus, newValue : PlayerStatus, playerId : int) =
    inherit ChangeEffectDto<PlayerStatus>(EffectKind.PlayerStatusChanged, oldValue, newValue)
    [<Required>]
    member __.PlayerId = playerId

and TurnCyclePlayerFellFromPowerEffectDto(oldValue : List<int>, newValue : List<int>, playerId : int) =
    inherit ChangeEffectDto<List<int>>(EffectKind.TurnCyclePlayerFellFromPower, oldValue, newValue)
    [<Required>]
    member __.PlayerId = playerId

and TurnCyclePlayerRemovedEffectDto(oldValue : List<int>, newValue : List<int>, playerId : int) =
    inherit ChangeEffectDto<List<int>>(EffectKind.TurnCyclePlayerRemoved, oldValue, newValue)
    [<Required>]
    member __.PlayerId = playerId

and TurnCyclePlayerRoseToPowerEffectDto(oldValue : List<int>, newValue : List<int>, playerId : int) =
    inherit ChangeEffectDto<List<int>>(EffectKind.TurnCyclePlayerRoseToPower, oldValue, newValue)
    [<Required>]
    member __.PlayerId = playerId

type EventDto = {
    [<Required>]
    id : int

    [<Required>]
    createdBy : CreationSourceDto
    
    actingPlayerId : Nullable<int>
    
    [<Required>]
    kind : EventKind

    [<Required>]
    effects : List<EffectDto>
}

type StateAndEventResponseDto = {
    [<Required>]
    game : GameDto
    
    [<Required>]
    event : EventDto
}
