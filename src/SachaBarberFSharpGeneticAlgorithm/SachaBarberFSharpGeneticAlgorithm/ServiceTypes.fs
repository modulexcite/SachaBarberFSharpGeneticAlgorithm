namespace SachaBarberFSharpGeneticAlgorithm

open System
open System.Reactive.Subjects
open System.Reactive.Linq
open SachaBarberFSharpGeneticAlgorithm.ModelTypes


[<AutoOpen>]
module ServiceTypes = 

    type IPopulationInitialiser =
        abstract member InitialPopulationStream : IObservable<seq<SachaBarberFSharpGeneticAlgorithm.ModelTypes.Person>>
        abstract member PublishPopulation : seq<SachaBarberFSharpGeneticAlgorithm.ModelTypes.Person> -> unit

    type PopulationInitialiser () =
        let initialPopulationSubject = new ReplaySubject<seq<SachaBarberFSharpGeneticAlgorithm.ModelTypes.Person>>()
        interface IPopulationInitialiser with
            member this.InitialPopulationStream
                with get () = initialPopulationSubject.AsObservable()
            member this.PublishPopulation(population:seq<SachaBarberFSharpGeneticAlgorithm.ModelTypes.Person>) = 
                initialPopulationSubject.OnNext(population)
