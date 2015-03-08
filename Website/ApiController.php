<?php

class Status {
	public $code     = "";
	public $response = "";
}

class User {
    public  $guid       = "";
    public  $username   = "";
    public  $creation   = "";
    public  $lastlogin  = "";
    public  $email      = "";
    private $token      = "";
}

class Application {
    public  $guid            = "";
	private $token           = ""; //private variables don't get returned in json_encode. NEVER RETURN token FOR SECURITY REASONS
    public  $name            = "";
    public  $user            = "";
    public  $localip         = "";
	public  $localport       = "";
	public  $remoteip        = "";
	public  $remoteport      = "";
    public  $lastconnection  = "";
    public  $updatedate      = "";
}

function success($message)
{
	//$message->code = 200;
	return $message;
}

function status($code, $message)
{
	$status = new Status();
	$status->code = $code;
	$status->response = $message;
	return $status;
}

//Get a guid
function getGuid()
{
	//Connect to Database
	$connectionOptions = array("Database"=>"SyncUp", "UID"=>"test", "PWD"=>"test");

	$conn = sqlsrv_connect("localhost", $connectionOptions);
	if ($conn === false) {
		die(print_r( sqlsrv_errors(), true));
	}

	//Create a new GUID in the database
	$sql = "SELECT NEWID() as guid";
	$stmt = sqlsrv_query(&$conn, $sql);
	if( $stmt === false) {
		return json_encode(status(500, sqlsrv_errors()));
	}

	if(sqlsrv_has_rows($stmt) === true)
	{
		$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
		$response->guid = $row['guid'];
		return json_encode(success($response));
	}
}

// Registers a user
function registerUser($username, $password, $email)
{
	if(empty($username) || empty($password))
	{
		return json_encode(status(400, "Invalid Parameters"));
	}
	
	//Connect to Database
	$connectionOptions = array("Database"=>"SyncUp", "UID"=>"test", "PWD"=>"test");

	$conn = sqlsrv_connect("localhost", $connectionOptions);
	if ($conn === false) {
		die(print_r( sqlsrv_errors(), true));
	}
	
	//Check if username is available
	$sql = "SELECT userid FROM Users WHERE username = ?";
	$stmt = sqlsrv_query(&$conn, $sql, array( &$username));
	if( $stmt === false) {
		return json_encode(status(500, sqlsrv_errors()));
	}

	if(sqlsrv_has_rows($stmt) === false)
	{
		// Username is available, insert it into the database
		$salt = "1234567890";

		$sql = "INSERT INTO Users (username, password, salt, email) VALUES (?, ?, ?, ?)";
		$stmt = sqlsrv_query(&$conn, $sql, array( &$username, &$password, &$salt, &$email));
		if( $stmt === false) {
			return json_encode(status(501, sqlsrv_errors()));
		}
		return json_encode(status(200, "Success"));
	}
	else
	{
		return json_encode(status(203, "Non-Authoritative Information"));
	}
}

// for testing
function authenticate($username, $password)
{
	if(!empty($username) && !empty($password))
	{
		//Connect to Database
		$connectionOptions = array("Database"=>"SyncUp", "UID"=>"test", "PWD"=>"test");

		$conn = sqlsrv_connect("localhost", $connectionOptions);
		if ($conn === false) {
			die(print_r( sqlsrv_errors(), true));
		}
	
		//Need to apply encryption to password to compare to database
		$sql = "SELECT * FROM Users WHERE username = ? AND password = ?";
		$stmt = sqlsrv_query(&$conn, $sql, array( &$username, &$password));
		if( $stmt === false) {
			return json_encode(status(500, sqlsrv_errors()));
		}

		if(sqlsrv_has_rows($stmt) === true)
		{
			$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

			$user = new User();
			$user->guid= $row['userid'];
			$user->username= $row['username'];
			$user->creation= $row['creation'];
			$user->lastlogin= $row['lastlogin'];
			$user->email= $row['email'];
			//$user->token= $row['token'];

			return json_encode(success($user));
		}
		else
		{
			return json_encode(status(203, "Non-Authoritative Information"));
		}
	}
	else
	{
		return json_encode(status(400, "Invalid Parameters"));
	}
}

// Registers an app with a given guid
function registerAppInstance($username, $password, $guid, $name)
{
	if(empty($username) || empty($password) || empty($guid) || empty($name))
	{
		return json_encode(status(400, "Invalid Parameters"));
	}
	
	//Connect to Database
	$connectionOptions = array("Database"=>"SyncUp", "UID"=>"test", "PWD"=>"test");

	$conn = sqlsrv_connect("localhost", $connectionOptions);
	if ($conn === false) {
		die(print_r( sqlsrv_errors(), true));
	}
	
	//Need to apply encryption to password to compare to database
	$sql = "SELECT * FROM Users WHERE username = ? AND password = ?";
	$stmt = sqlsrv_query(&$conn, $sql, array( &$username, &$password));
	if( $stmt === false) {
		return json_encode(status(500, sqlsrv_errors()));
	}

	if(sqlsrv_has_rows($stmt) === true)
	{
		$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
		
		$length = 32; //32 character security token
		$token = base64_encode(openssl_random_pseudo_bytes(3 * ($length >> 2)));

		$sql = "INSERT INTO Applications (appid, userid, token, name) VALUES (?, ?, ?, ?)";
		$stmt = sqlsrv_query(&$conn, $sql, array( &$guid, &$row['userid'], &$token, &$name));
		if( $stmt === false) {
			return json_encode(status(501, sqlsrv_errors()));
		}
		return json_encode(status(200, "Success"));
	}
	else
	{
		return json_encode(status(203, "Non-Authoritative Information"));
	}
}

function getAuthToken($username, $password, $appid)
{
	if(empty($username) || empty($password) || empty($appid))
	{
		return json_encode(status(400, "Invalid Parameters"));
	}
	
	//Connect to Database
	$connectionOptions = array("Database"=>"SyncUp", "UID"=>"test", "PWD"=>"test");

	$conn = sqlsrv_connect("localhost", $connectionOptions);
	if ($conn === false) {
		die(print_r( sqlsrv_errors(), true));
	}

	//Need to apply encryption to password to compare to database
	$sql = "SELECT * FROM Users WHERE username = ? AND password = ?";
	$stmt = sqlsrv_query(&$conn, $sql, array( &$username, &$password));
	if( $stmt === false) {
		return json_encode(status(500, sqlsrv_errors()));
	}

	if(sqlsrv_has_rows($stmt) === true)
	{
		$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
		$userid = $row['userid'];
		
		$sql = "SELECT token, userid FROM Applications WHERE appid = ?";
		$stmt = sqlsrv_query(&$conn, $sql, array( &$appid ));
		if( $stmt === false) {
			return json_encode(status(500, sqlsrv_errors()));
		}
		
		if(sqlsrv_has_rows($stmt) === true)
		{
			$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
			// Verify that the userid matches
			if ($userid == $row['userid'])
			{
				// Return token
				$response->token = $row['token'];
				return json_encode(success($response));
			}
			else
			{
				return json_encode(status(206, "Invalid App Id (user doesn't match)"));
			}
		}
		else
		{
			return json_encode(status(206, "Invalid App Id"));
		}
	}
	else
	{
		return json_encode(status(203, "Non-Authoritative Information"));
	}
}

function verifyAppWithToken($appid, $token)
{
	//Connect to Database
	$connectionOptions = array("Database"=>"SyncUp", "UID"=>"test", "PWD"=>"test");

	$conn = sqlsrv_connect("localhost", $connectionOptions);
	if ($conn === false) {
		die(print_r( sqlsrv_errors(), true));
	}
	
	$sql = "SELECT * FROM Applications WHERE appid = ? AND token = ?";
	$stmt = sqlsrv_query(&$conn, $sql, array( &$appid, &$token));
	if( $stmt === false) {
		return json_encode(status(500, sqlsrv_errors()));
	}

	if(sqlsrv_has_rows($stmt) === true)
	{
		$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
		if ($row['appid'] == $appid)
		{
			return 200;
		}
		return 206;
	}
	return 203;
}

function sendMessage($appid, $token, $userid, $msg)
{
	if(empty($userid) || empty($token) || empty($appid) || empty($msg))
	{
		return json_encode(status(400, "Invalid Parameters"));
	}

	$error = verifyAppWithToken($appid, $token);
	if (error != 200)
	{
		return json_encode(status(error, "error validating user"));
	}
	
	//Connect to Database
	$connectionOptions = array("Database"=>"SyncUp", "UID"=>"test", "PWD"=>"test");

	$conn = sqlsrv_connect("localhost", $connectionOptions);
	if ($conn === false) {
		die(print_r( sqlsrv_errors(), true));
	}
	
	$sql = "INSERT INTO Messages (fromit, toid, msg) WHERE (SELECT userid FROM Applications WHERE appid = ? AND token = ?, ?, ?)";
	$stmt = sqlsrv_query(&$conn, $sql, array( &$appid, &$token, &$userid, &$msg));
	if( $stmt === false) {
		return json_encode(status(500, sqlsrv_errors()));
	}
	return json_encode(success("Success"));
}

?>