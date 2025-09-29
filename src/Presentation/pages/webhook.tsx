import Aside from "components/ui/aside";
import { http } from "services/http";
import type { GetWebhookResponse, RequestResponse } from "types/types";
import type { Route } from "./+types/webhook";
import { format } from "@helpers/formatter";
import MainLayout from "@components/ui/main-layout";
import { useState } from "react";
import { useRevalidator } from "react-router";
import { toast } from "@helpers/toast";

export function meta({}: Route.MetaArgs) {
  return [
    { title: "Simple Webhook" },
    { name: "description", content: "Simple Webhook Site" },
  ];
}

export async function clientLoader({ params }: Route.ClientLoaderArgs) {
  const data = await http.get(`/api/webhooks/${params.id}`) as GetWebhookResponse;
  return data;
}

export default function Home({ loaderData }: Route.ComponentProps) {
  const colors = {
    "GET": "#28a745",
    "POST": "#4E32B2",
    "PUT": "#28a745",
    "PATCH": "#ffc107",
    "DELETE": "#dc3545",
    "HEAD": "#17a2b8",
    "OPTIONS": "#6c757d",
  };
  const [selected, setSelected] = useState<RequestResponse|null>();
  const [headerDisplay, setHeaderDisplay] = useState<string>("none");
  const revalidator = useRevalidator();

  const handleHeaderToggle = () => {
    setHeaderDisplay((prev) => prev == "none" ? "block" : "none")
  }

  const handleRefresh = () => {
    revalidator.revalidate();
    toast("Updated", { type: "success" })
  }

  return (
    <MainLayout>
      <Aside>
        <nav className="flex flex-col px-5">
          { loaderData && loaderData.requests.length == 0 && (
            <p className="text-center">Requests will be here</p>
          )}
          { loaderData && loaderData.requests.map((request, i) => (
            <div key={i} className="shadow mb-4 px-4 py-2 rounded cursor-pointer" onClick={() => setSelected(request)}>
              <p>
                <span style={{background: `${colors[request.method]}`}} className="px-2 py-1 rounded text-white text-sm">{request.method}</span>
                <span className="ml-2">{request.sourceIp} </span>
              </p>
              <p className="text-sm pt-1">{format.date(request.receivedAt)}</p>
            </div>
          ))}
        </nav>
      </Aside>
      <main className="p-5">
        <div className="flex justify-between py-5">
          <h1>Request</h1>
          <button onClick={handleRefresh} className="bg-[var(--primary)] text-white px-4 text-sm py-2 rounded cursor-pointer flex gap-2" type="submit">
            <svg width="20px" height="20px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M13 2L11 3.99545L11.0592 4.05474M11 18.0001L13 19.9108L12.9703 19.9417M11.0592 4.05474L13 6M11.0592 4.05474C11.3677 4.01859 11.6817 4 12 4C16.4183 4 20 7.58172 20 12C20 14.5264 18.8289 16.7793 17 18.2454M7 5.75463C5.17107 7.22075 4 9.47362 4 12C4 16.4183 7.58172 20 12 20C12.3284 20 12.6523 19.9802 12.9703 19.9417M11 22.0001L12.9703 19.9417" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
            </svg>
            <p>Refresh</p>
            </button>
        </div>
        { selected && (
          <div>
            <button type="button" className="flex justify-between w-full cursor-pointer bg-[var(--primary-bg)] text-[var(--primary-fg)] py-2 px-4 rounded" onClick={handleHeaderToggle}>
              <p>Headers</p>
              <svg width="24px" height="24px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path d="M6 12H18M12 6V18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
              </svg>
            </button>
            <div className="pt-2" style={{display: headerDisplay}}>
              { selected.queryParameters && (
                <p>
                  <span className="font-bold text-sm">QueryString: </span>{selected.queryParameters}
                </p>
              )}
              { Object.keys(selected!.headers).map((header, i) => (
                <p key={i}>
                  <span className="font-bold text-sm">{header}: </span>{selected.headers[header]}
                </p>
              ))}
            </div>
            <div className="mt-5 text-sm border overflow-auto p-2 rounded border-[var(--background-container)]">
              <pre>
                { selected.body }
              </pre>
            </div>
          </div>
        )}
      </main>
    </MainLayout>
  )
}
