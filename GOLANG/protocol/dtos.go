package main

type RequestDTO struct {
	ProtocolMessage string `json:"protocolMessage"`
}

type ResponseDTO struct {
	Answer string `json:"answer"`
}
