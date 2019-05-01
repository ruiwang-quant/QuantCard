    
using System;
using System.Collections;
using System.Collections.Generic;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using UnityEngine;

public class QuantCardContract {

    public static string ABI = @"[{'constant': false,'inputs': [{'name': 'description','type': 'string'},{'name': 'timestamp','type': 'uint256'}],'name': 'publishNews','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [],'name': '_owner','outputs': [{'name': '','type': 'address'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': true,'inputs': [{'name': 'index','type': 'uint8'}],'name': 'getNews','outputs': [{'name': '','type': 'string'},{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': '_newOwner','type': 'address'}],'name': 'transferOwnership','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'inputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'constructor'},{'anonymous': false,'inputs': [{'indexed': false,'name': 'previousOwner','type': 'address'},{'indexed': false,'name': 'newOwner','type': 'address'}],'name': 'OwnershipTransferred','type': 'event'}]";
    private static string CONTRACT = "0x79de779489c864f7a31960a9C8bd2EB6E65Bd67A";
    private Contract contract;

    public QuantCardContract () {
        this.contract = new Contract (null, ABI, CONTRACT);
    }

    public Function getNewsFunction () {
        return this.contract.GetFunction("getNews");
    }

    public CallInput createGetNewsCallInput (int index) {
        var function = getNewsFunction ();
        return function.CreateCallInput (index);
    }

    public GetNewsFunctionOutput DecodeGetNewsDTO (string result) {
        var function = getNewsFunction ();
        return function.DecodeDTOTypeOutput<GetNewsFunctionOutput> (result);
    }

    [FunctionOutput]
    public class GetNewsFunctionOutput : IFunctionOutputDTO
    {
		[Parameter("string", 1)]
        public string Description { get; set; }

        [Parameter("uint256", 2)]
        public long Timestamp { get; set; }
    }

}
