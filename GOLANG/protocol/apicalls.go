package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"net/http"
)

const (
	AuthToken = "Team 17BC43C7-5CE5-4FBF-9AE5-1D3C1C66AA87"
	BaseURL   = "https://exs-htf-2024.azurewebsites.net"
)

func apiGet[T any](endpoint string) (T, error) {
	var result T
	client := &http.Client{}

	req, err := http.NewRequest("GET", BaseURL+endpoint, nil)
	if err != nil {
		return result, fmt.Errorf("failed to create request: %v", err)
	}

	req.Header.Add("Authorization", AuthToken)

	resp, err := client.Do(req)
	if err != nil {
		return result, fmt.Errorf("HTTP request error: %v", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return result, fmt.Errorf("unexpected status code: %d", resp.StatusCode)
	}

	body, err := io.ReadAll(resp.Body)
	if err != nil {
		return result, fmt.Errorf("failed to read response body: %v", err)
	}

	err = json.Unmarshal(body, &result)
	if err != nil {
		return result, fmt.Errorf("JSON deserialization error: %v", err)
	}

	return result, nil
}

func apiPost[T any](endpoint string, body T) error {
	client := &http.Client{}

	jsonBody, err := json.Marshal(body)
	if err != nil {
		return fmt.Errorf("failed to serialize body: %v", err)
	}

	req, err := http.NewRequest("POST", BaseURL+endpoint, bytes.NewBuffer(jsonBody))
	if err != nil {
		return fmt.Errorf("failed to create request: %v", err)
	}

	req.Header.Add("Authorization", AuthToken)
	req.Header.Add("Content-Type", "application/json")

	resp, err := client.Do(req)
	if err != nil {
		return fmt.Errorf("HTTP request error: %v", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return fmt.Errorf("unexpected status code: %d", resp.StatusCode)
	}

	return nil
}
