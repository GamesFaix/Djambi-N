namespace Djambi.Api.Db.Repositories

open System.Data.Entity.Core
open System.Linq
open Microsoft.EntityFrameworkCore
open Djambi.Api.Db.Interfaces
open Djambi.Api.Db.Mappings
open Djambi.Api.Db.Model
open Djambi.Api.Common

type GameRepository(context : DjambiDbContext) =
    let maybeSave (commit : bool) =
        task {
            if commit
            then 
                let! _ = context.SaveChangesAsync()
                return ()
            else
                return ()
        }

    interface IGameRepository with
        member __.getGame gameId =
            task {
                let! g = 
                    context.Games
                        .Include(fun g -> g.Players)
                        .Include(fun g -> g.CreatedByUser)
                        .SingleOrDefaultAsync(fun g -> g.GameId = gameId)
                if g = null
                then return raise <| ObjectNotFoundException("Game not found.")
                else 
                    g.Players <- g.Players.OrderBy(fun p -> p.PlayerId).ToList()
                    return g |> toGame
            }
            
        member __.createGame(request, ?commit) =
            let commit = defaultArg commit false
            task {
                let g = request |> toGameSqlModel
                let! _ = context.Games.AddAsync(g)
                let! _ = maybeSave commit
                return g.GameId
            }
        
        member __.updateGame(game, ?commit) =
            let commit = defaultArg commit false
            task {
                let! g = context.Games.FindAsync(game.id)
                g.IsPublic <- game.parameters.isPublic
                g.AllowGuests <- game.parameters.allowGuests
                g.Description <- game.parameters.description |> Option.toObj
                g.RegionCount <- byte game.parameters.regionCount
                g.GameStatusId <- game.status
                g.CurrentTurnJson <- game.currentTurn |> Json.serialize
                g.TurnCycleJson <- game.turnCycle |> Json.serialize
                g.PiecesJson <- game.pieces |> Json.serialize
                context.Games.Update(g) |> ignore
                let! _ = maybeSave commit
                return ()
            }

        member __.getNeutralPlayerNames () =
            task {
                let! names = 
                    context.NeutralPlayerNames
                        .Select(fun x -> x.Name)
                        .ToListAsync()

                return names |> Seq.toList
            }

        member __.createGameAndAddPlayer (gameRequest, playerRequest) =
            task {
                use! transaction = context.Database.BeginTransactionAsync()

                let gameSqlModel = toGameSqlModel gameRequest
                let! _ = context.Games.AddAsync(gameSqlModel)
                let! _ = context. SaveChangesAsync()

                let playerSqlModel = createPlayerRequestToPlayerSqlModel playerRequest
                playerSqlModel.GameId <- gameSqlModel.GameId
                let! _ = context.Players.AddAsync(playerSqlModel)

                let! _ = context.SaveChangesAsync()
                let! _ = transaction.CommitAsync()

                return gameSqlModel.GameId
            }
