using System.Collections;
using System.Collections.Generic;
using TMPro;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.JsonRpc.UnityClient;
using UnityEngine;

public class CardManager : MonoBehaviour
{

	public Animator quantLogoRotationAnimator;
	public TextMeshProUGUI newsTextMesh;
	// private static string ADDRESS = "0x0542091eF2fC32ACBa0EdCa930aDD21AFa055d9E";
	// private static string CONTRACT = "0x79de779489c864f7a31960a9C8bd2EB6E65Bd67A";
	private static string ETH_ENDPOINT = "https://ropsten.infura.io/v3/88f2c6909fa348ed85fb0a68bf27fde3";
	private bool isAnimating = false;
	private QuantCardContract _quantCardContract;
	private string message = "Quant";
    
	void Start () {
		this._quantCardContract = new QuantCardContract();
		this.quantLogoRotationAnimator.GetComponent<Animator>();
		this.newsTextMesh.GetComponent<TextMeshProUGUI>();

		StartCoroutine(getNews());
	}

	public IEnumerator getNews () {
		var getNewsRequest = new EthCallUnityRequest (ETH_ENDPOINT);
		
		var getNewsCallInput = this._quantCardContract.createGetNewsCallInput(0);
		
		yield return getNewsRequest.SendRequest (getNewsCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		var value = this._quantCardContract.DecodeGetNewsDTO (getNewsRequest.Result);

		this.message += value.Description + "\n\n";


		getNewsCallInput = this._quantCardContract.createGetNewsCallInput(1);
		
		yield return getNewsRequest.SendRequest (getNewsCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		value = this._quantCardContract.DecodeGetNewsDTO (getNewsRequest.Result);

		if (null != value && null != value.Description) {
			this.message += value.Description + "\n\n";
		}
		
		getNewsCallInput = this._quantCardContract.createGetNewsCallInput(2);
		
		yield return getNewsRequest.SendRequest (getNewsCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		value = this._quantCardContract.DecodeGetNewsDTO (getNewsRequest.Result);

		if (null != value && null != value.Description) {
			this.message += value.Description + "\n\n";
		}

		this.newsTextMesh.text = this.message;
	}

	// We create the function which will check the balance of the address and return a callback with a decimal variable
	public static IEnumerator getAccountBalance (string address, System.Action<decimal> callback) {
		// Now we define a new EthGetBalanceUnityRequest and send it the testnet url where we are going to
		// check the address, in this case "https://kovan.infura.io".
		// (we get EthGetBalanceUnityRequest from the Netherum lib imported at the start)
		var getBalanceRequest = new EthGetBalanceUnityRequest (ETH_ENDPOINT);
		// Then we call the method SendRequest() from the getBalanceRequest we created
		// with the address and the newest created block.
		yield return getBalanceRequest.SendRequest(address, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());
		
		// Now we check if the request has an exception
		if (getBalanceRequest.Exception == null) {
			// We define balance and assign the value that the getBalanceRequest gave us.
			var balance = getBalanceRequest.Result.Value;
			// Finally we execute the callback and we use the Netherum.Util.UnitConversion
			// to convert the balance from WEI to ETHER (that has 18 decimal places)
			callback (Nethereum.Util.UnitConversion.Convert.FromWei(balance, 18));
		} else {
			// If there was an error we just throw an exception.
			throw new System.InvalidOperationException ("Get balance request failed");
		}

	}

    // Update is called once per frame
    void Update()
    {
        if (!this.isAnimating) {
			this.quantLogoRotationAnimator.Play("QuantLogoRotation");
			this.isAnimating = true;

			// this.newsTextMesh.text = "1234\n5678";
		}
    }
	
}
