import Aside from "components/ui/aside";
import { http } from "services/http";
import type { GetWebhookResponse, RequestResponse } from "types/types";
import type { Route } from "./+types/webhook";
import { format } from "@helpers/formatter";
import MainLayout from "@components/ui/main-layout";
import { useState } from "react";

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

  const handleHeaderToggle = () => {
    setHeaderDisplay((prev) => prev == "none" ? "block" : "none")
  }

  return (
    <MainLayout>
      <Aside>
        <nav className="flex flex-col px-5">
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
