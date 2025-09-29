import type { PropsWithChildren } from "react";
import { Link } from "react-router";

export default function Aside({ children }: PropsWithChildren) {
  return (
    <aside>
      <div className="absolute md:hidden top-0 right-0">
        <button id="button-menu" onClick={() => document.querySelector("body")?.classList.toggle("toggle-aside")} className="p-4 cursor-pointer">
          <svg width="24px" height="24px" viewBox="0 0 16 16" xmlns="http://www.w3.org/2000/svg" fill="none"><path fill="currentColor" fillRule="evenodd" d="M3.25 1A2.25 2.25 0 001 3.25v9.5A2.25 2.25 0 003.25 15h9.5A2.25 2.25 0 0015 12.75v-9.5A2.25 2.25 0 0012.75 1h-9.5zM2.5 3.25a.75.75 0 01.75-.75h1.8v11h-1.8a.75.75 0 01-.75-.75v-9.5zM6.45 13.5h6.3a.75.75 0 00.75-.75v-9.5a.75.75 0 00-.75-.75h-6.3v11z" clipRule="evenodd"/></svg>          
        </button>
      </div>
      <Link to={"/"}>
        <section className="flex justify-center py-6 border-b border-[var(--border)] mb-5">
          <img src="/logo.png" alt="Simple Webhook" className="h-14" />
        </section>
      </Link>
      {children}
    </aside>
  )
}
