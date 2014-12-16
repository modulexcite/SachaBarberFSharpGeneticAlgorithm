﻿module main

open System
open FsXaml
open log4net
open FsIOCWindow.ContainerTypes
open FsIOCWindow.ServiceTypes
open FsIOCWindow.Initialisation

type App = XAML<"App.xaml">

[<STAThread>]
[<EntryPoint>]
let main argv =

    let log = LogManager.GetLogger(Operators.typeof<App>)
    log4net.Config.XmlConfigurator.Configure() |> ignore

    try
        let population =  DoInitialPopulationLoadingStrategy()
        log.Info("Sucessfully loaded population from chosen population initialiser")
        printPopulation(population)
        FsIOCWindow.Initialisation.InitialiseIOC()
        IOCManager.Instance.Container.Resolve<IPopulationInitialiser>()
            .PublishPopulation("YO ITS WOrKING") |> ignore
        App().Root.Run()
    with
        | Failure(msg) -> 
            log.Error("Error starting up, this could be caused by by AppSettings value for 'InitialPopulationLoadingStrategy'")
            printUsage() 


