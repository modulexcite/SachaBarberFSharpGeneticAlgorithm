namespace FsIOCWindow
module Initialisation = 

    open Deedle
    open System
    open System.Configuration
    open FSharp.Data


    //Person record
    type Person = 
        {   Age:int; 
            Name:string 
        }
        override this.ToString() = String.Format("Age : {0}, Name: {1}", this.Age, this.Name)


    type initialPopulationDataFromCsv = CsvProvider<"InitialPopulation.csv">


    let createInitialPopulationFromCsvUsingCsvProvider() = 
        let exePath = System.Reflection.Assembly.GetEntryAssembly().Location
        let path = exePath.Substring(0,exePath.LastIndexOf(@"\") + 1) + "InitialPopulation.csv"
        let populationFromCsv = initialPopulationDataFromCsv.Load(path)
        populationFromCsv.Rows |> Seq.map (fun row -> 
            { 
                Age=Convert.ToInt32(row.Age);
                Name=row.Name
            })

    let createInitialPopulationFromCsvUsingDeedle() = 
        let exePath = System.Reflection.Assembly.GetEntryAssembly().Location
        let path = exePath.Substring(0,exePath.LastIndexOf(@"\") + 1) + "InitialPopulation.csv"
        let testCsv = Frame.ReadCsv(path)
        let loadedPopulation  = testCsv |> Frame.mapRowValues (fun row -> 
            { 
                Age=Convert.ToInt32(row?Age);
                Name=row.["Name"].ToString()
            })
        loadedPopulation.Values


    let printUsage() =
        printfn "SachaBarberFSharpFinalProject must run with either 'Deedle' 
                    or 'CSVProvider'\r\nfor the InitialPopulationLoadingStrategy App.Config setting"
        -1 

    let DoInitialPopulationLoadingStrategy() =
        let initialPopulationProviderType = 
            ConfigurationManager.AppSettings.Item("InitialPopulationLoadingStrategy")
        match initialPopulationProviderType with
            | "Deedle" -> createInitialPopulationFromCsvUsingDeedle()
            | "CSVProvider" ->   createInitialPopulationFromCsvUsingCsvProvider()
            | _ -> failwith  "Unknown InitialPopulationLoadingStrategy" 


    let printPopulation(population) =
        for popItem in population do
            printfn "%O" popItem



    let InitialiseIOC() =
        IOCManager.Instance.Container.Register<IPopulationInitialiser>(
            typeof<PopulationInitialiser>, Singleton)
        ()

