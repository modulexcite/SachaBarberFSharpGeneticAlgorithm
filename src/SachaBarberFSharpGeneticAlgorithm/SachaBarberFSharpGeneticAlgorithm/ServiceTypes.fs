namespace FsIOCWindow

open System
open System.Reactive.Subjects
open System.Reactive.Linq


[<AutoOpen>]
module ServiceTypes = 

    type IPopulationInitialiser =
        abstract member InitialPopulationStream : IObservable<string>
        abstract member PublishPopulation : string -> unit

    type PopulationInitialiser () =
        let initialPopulationSubject = new ReplaySubject<string>()
        interface IPopulationInitialiser with
            member this.InitialPopulationStream
                with get () = initialPopulationSubject.AsObservable()
            member this.PublishPopulation(population:string) = 
                initialPopulationSubject.OnNext(population)
