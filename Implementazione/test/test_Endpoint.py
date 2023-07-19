import unittest
from Endpoint import Endpoint
from unittest.mock import patch
from unittest.mock import MagicMock, Mock, patch
from requests.exceptions import RequestException
from io import StringIO
import sys
sys.path.append('C:\\Users\\auror\\source\\repos\\Admin_Client')


admin_id="Admin12345"

class EndpointTest(unittest.TestCase):
    def setUp(self):
      self.endpoint = Endpoint(admin_id)
    
      #caso successs
    @patch('endpoint.requests.post')
    def test_data_to_server_success(self, mock_post):
        endpoint = "/test"
        data = {"key": "value"}
        expected_url = "http://localhost:18080" + endpoint
        expected_response_text = "Test response"

        mock_response = mock_post.return_value
        mock_response.raise_for_status.return_value = None
        mock_response.text = expected_response_text

        response = self.endpoint.data_to_server(endpoint, data)

        mock_post.assert_called_once_with(expected_url, headers={'Content-Type': 'application/json'}, json=data)
        mock_response.raise_for_status.assert_called_once()
        self.assertEqual(response, expected_response_text)

    @patch('endpoint.requests.post')
    def test_data_to_server_request_exception(self, mock_post):
        endpoint = "/test"
        data = {"key": "value"}

        mock_post.side_effect = RequestException("Test exception")

        with self.assertRaises(RequestException):
            self.endpoint.data_to_server(endpoint, data)
            mock_post.assert_called_once()

    @patch('builtins.input', return_value='valid_input')
    def test_get_non_empty_input_valid(self, mock_input):
        prompt = "Enter a non-empty value: "
        expected_output = 'valid_input'

        with patch('sys.stdout', new=StringIO()) as fake_output:
            result = self.endpoint.get_non_empty_input(prompt)

        self.assertEqual(result, expected_output)
        self.assertEqual(fake_output.getvalue(), '')

    @patch('builtins.input', side_effect=['', '   ', 'valid_input'])
    def test_get_non_empty_invalid(self, mock_input):
        prompt = "Enter a non-empty value: "
        expected_output = 'valid_input'

        with patch('sys.stdout', new=StringIO()) as fake_output:
            result = self.endpoint.get_non_empty_input(prompt)

        self.assertEqual(result, expected_output)
        self.assertEqual(fake_output.getvalue(), "Input non valido. Riprova.\n" * 2)

    @patch('endpoint.requests.post')
    def test_get_occupied_spots_all_floors(self, mock_post):
        #risposta simulati
        endpoint = "/spots"  #Piano: 1, con posti: 50 su 50
        expected_response = '{"Piano: 1,con posti: 10, su 50"}'
        mock_response = mock_post.return_value
        mock_response.text = expected_response

        # Chiamiamo il metodo e per ott il risultato
        result = self.endpoint.get_occupied_spots_all_floors()

        # chiamata mock al metodo 
        mock_post.assert_called_once_with("http://localhost:18080/spots", headers={'Content-Type': 'application/json'}, json=None)

        # risultato ottenuto
        self.assertEqual(result, "Piano: 1,con posti: 10, su 50")


if __name__ == '__main__':
    unittest.main()