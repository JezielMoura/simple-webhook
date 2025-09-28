import type { PropsWithChildren } from "react";
import { Link } from "react-router";

export default function Aside({ children }: PropsWithChildren) {
  return (
    <aside>
      <Link to={"/"}>
        <section className="flex justify-center py-5 border-b border-[var(--background-container)] mb-5">
          <img src="/logo.png" alt="Simple Webhook" className="h-16" />
        </section>
      </Link>
      {children}
    </aside>
  )
}
