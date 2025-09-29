import Aside from "components/ui/aside";
import type { Route } from "./+types/home";
import { http } from "services/http";
import type { GetWebhookResponse, ListWebhookResponse } from "types/types";
import MainLayout from "@components/ui/main-layout";
import { Link, useRevalidator } from "react-router";
import { toast } from "@helpers/toast";

export function meta({}: Route.MetaArgs) {
  return [
    { title: "Simple Webhook" },
    { name: "description", content: "Simple Webhook Site" },
  ];
}

export async function clientLoader() {
  return await http.get("/api/webhooks") as ListWebhookResponse;
}

export default function Home({ loaderData }: Route.ComponentProps) {
  const revalidator = useRevalidator();

  const handleCreate = async () => {
    const secret = prompt("Put a secret or empty for public access", "");
    const url = await http.post("/api/webhooks", { secret }) as string;
    localStorage.setItem("secret", secret || "");
    setTimeout(async () => {
      await navigator.clipboard.writeText(url);
      toast("Webhook criado e url copiada para área de transferência", { type: "success" });
      await revalidator.revalidate();
    }, 1000)
  }

  const handleCopy = async (url: string) => {
    await navigator.clipboard.writeText(url);
    toast("Webhook copiado para área de transferência", { type: "success" })
  }

  const handleDelete = async (webhook: GetWebhookResponse) => {
    if(confirm(`Deseja prosseguir com a exclusão do webhook ${webhook.url}`)) {
      const success = await http.del(`/api/webhooks/${webhook.id}`)
      if (success) {
        toast("Exlusão do webhook efetuada", { type: "success" })
        await revalidator.revalidate();
      }
    }
  }

  return (
    <MainLayout>
      <Aside>
        <p className="p-4 text-center">Select your wehbook, requests will be here</p>
      </Aside>
      <main className="flex flex-col px-5  max-[800px] col-start-1">
        <div className="flex justify-between py-5">
          <h1>Webhooks</h1>
          <button onClick={handleCreate} className="bg-[var(--primary)] text-white px-4 text-sm py-2 rounded cursor-pointer" type="submit">Create Webhook</button>
        </div>
        { loaderData.webhooks.map((webhook, i) => (
          <div key={i} className="flex justify-between items-center shadow mb-4 px-4 py-2 rounded">
            <Link to={`webhook/${webhook.id}`}>
              <p className="text-sm pt-1">{webhook.url}</p>
            </Link>
            <div className="flex gap-2">
              <button className="cursor-pointer" onClick={() => handleCopy(webhook.url)}>
                <svg width="24px" height="24px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M19.53 8L14 2.47C13.8595 2.32931 13.6688 2.25018 13.47 2.25H11C10.2707 2.25 9.57118 2.53973 9.05546 3.05546C8.53973 3.57118 8.25 4.27065 8.25 5V6.25H7C6.27065 6.25 5.57118 6.53973 5.05546 7.05546C4.53973 7.57118 4.25 8.27065 4.25 9V19C4.25 19.7293 4.53973 20.4288 5.05546 20.9445C5.57118 21.4603 6.27065 21.75 7 21.75H14C14.7293 21.75 15.4288 21.4603 15.9445 20.9445C16.4603 20.4288 16.75 19.7293 16.75 19V17.75H17C17.7293 17.75 18.4288 17.4603 18.9445 16.9445C19.4603 16.4288 19.75 15.7293 19.75 15V8.5C19.7421 8.3116 19.6636 8.13309 19.53 8ZM14.25 4.81L17.19 7.75H14.25V4.81ZM15.25 19C15.25 19.3315 15.1183 19.6495 14.8839 19.8839C14.6495 20.1183 14.3315 20.25 14 20.25H7C6.66848 20.25 6.35054 20.1183 6.11612 19.8839C5.8817 19.6495 5.75 19.3315 5.75 19V9C5.75 8.66848 5.8817 8.35054 6.11612 8.11612C6.35054 7.8817 6.66848 7.75 7 7.75H8.25V15C8.25 15.7293 8.53973 16.4288 9.05546 16.9445C9.57118 17.4603 10.2707 17.75 11 17.75H15.25V19ZM17 16.25H11C10.6685 16.25 10.3505 16.1183 10.1161 15.8839C9.8817 15.6495 9.75 15.3315 9.75 15V5C9.75 4.66848 9.8817 4.35054 10.1161 4.11612C10.3505 3.8817 10.6685 3.75 11 3.75H12.75V8.5C12.7526 8.69811 12.8324 8.88737 12.9725 9.02747C13.1126 9.16756 13.3019 9.24741 13.5 9.25H18.25V15C18.25 15.3315 18.1183 15.6495 17.8839 15.8839C17.6495 16.1183 17.3315 16.25 17 16.25Z" fill="currentColor"/>
                </svg>
              </button>
              <button onClick={() => handleDelete(webhook)} className="cursor-pointer">
                <svg width="24px" height="24px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M4 6H20M16 6L15.7294 5.18807C15.4671 4.40125 15.3359 4.00784 15.0927 3.71698C14.8779 3.46013 14.6021 3.26132 14.2905 3.13878C13.9376 3 13.523 3 12.6936 3H11.3064C10.477 3 10.0624 3 9.70951 3.13878C9.39792 3.26132 9.12208 3.46013 8.90729 3.71698C8.66405 4.00784 8.53292 4.40125 8.27064 5.18807L8 6M18 6V16.2C18 17.8802 18 18.7202 17.673 19.362C17.3854 19.9265 16.9265 20.3854 16.362 20.673C15.7202 21 14.8802 21 13.2 21H10.8C9.11984 21 8.27976 21 7.63803 20.673C7.07354 20.3854 6.6146 19.9265 6.32698 19.362C6 18.7202 6 17.8802 6 16.2V6M14 10V17M10 10V17" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
              </button>
            </div>
          </div>
        ))}
      </main>
    </MainLayout>
  )
}
