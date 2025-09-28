export type ListWebhookResponse = {
    webhooks: GetWebhookResponse[];
}


export type GetWebhookResponse = {
  id:       string;
  url:      string;
  requests: RequestResponse[];
}

export type RequestResponse = {
  id:              string;
  webhookId:       string;
  method:          "GET" | "POST" | "PUT" | "PATCH" | "DELETE" | "OPTIONS" | "HEAD";
  headers:         DictionaryStringString;
  body:            string;
  queryParameters: string;
  sourceIp:        string;
  receivedAt:      Date;
}

type DictionaryStringString = { 
  readonly [key: string]: string 
}