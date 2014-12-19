namespace FsIOCWindow

open System
open System.Reactive.Subjects
open System.Reactive.Linq
open FsIOCWindow.ModelTypes


[<AutoOpen>]
module ServiceTypes = 

    type IPopulationInitialiser =
        abstract member InitialPopulationStream : IObservable<seq<FsIOCWindow.ModelTypes.Person>>
        abstract member PublishPopulation : seq<FsIOCWindow.ModelTypes.Person> -> unit

    type PopulationInitialiser () =
        let initialPopulationSubject = new ReplaySubject<seq<FsIOCWindow.ModelTypes.Person>>()
        interface IPopulationInitialiser with
            member this.InitialPopulationStream
                with get () = initialPopulationSubject.AsObservable()
            member this.PublishPopulation(population:seq<FsIOCWindow.ModelTypes.Person>) = 
                initialPopulationSubject.OnNext(population)
