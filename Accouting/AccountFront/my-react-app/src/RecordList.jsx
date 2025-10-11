import {useState, useEffect} from "react";

function RecordList(){
    const recordList = useRef([]);
    var showRecordList = [];

    useEffect(async () => {
        try{
            const res = await fetch("", {
                method: "POST",
                headers: "application/json",
                body: JSON.stringify({})
            });
            if(!res.ok) throw new Error("/Initialize/fail-to-fetch");
        }
        catch(e){

        }
    }, []);
}