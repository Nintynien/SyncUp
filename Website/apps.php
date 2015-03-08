<?php
include "ApiController.php"; //App API functions

// Get the action that's requested from the JSON post message
$method = $_SERVER['REQUEST_METHOD'];
$request = explode("/", substr(@$_SERVER['PATH_INFO'], 1));

switch ($method) {
  case 'PUT':
    put($request);  
    break;
  case 'POST':
    post($request);  
    break;
  case 'GET':
    get($request);  
    break;
  case 'HEAD':
    //rest_head($request);  
    //break;
  case 'DELETE':
    //rest_delete($request);  
    //break;
  case 'OPTIONS':
    //rest_options($request);    
    //break;
  default:
    //rest_error($request);
	echo status(404, "Invalid Request");
    break;
}

function put($request)
{
	echo "PUT";
	print_r($request);
}

function post($request)
{
	//echo "POST";
	//print_r($request);
	$raw_data = file_get_contents("php://input");
	if ($request[0] == "getuuid")
	{
		echo getGuid();
		return;
	}
	elseif ($request[0] == "login")
	{
		echo "login";
		echo authenticate("usera", "usera");
	}
	elseif ($request[0] == "register")
	{
		$json = json_decode($raw_data);
		echo registerAppInstance($json->username, $json->password, $json->guid, $json->name);
		return;
	}
	elseif ($request[0] == "gettoken")
	{
		$json = json_decode($raw_data);
		echo getAuthToken($json->username, $json->password, $json->guid);
		return;
	}
	echo $raw_data;
}

function get($request)
{
	//echo "GET";
	//print_r($request);
	$raw_data = file_get_contents("php://input");
	if ($request[0] == "getuuid")
	{
		echo getGuid();
		return;
	}
	elseif ($request[0] == "login")
	{
		echo "login";
		echo authenticate("usera", "usera");
	}
	elseif ($request[0] == "register")
	{
		echo "register";
	}
	elseif ($request[0] == "gettoken")
	{
		echo "gettoken";
	}
	echo $raw_data;
}

?>