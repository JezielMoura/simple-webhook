import type { PropsWithChildren } from "react";

export default function Header({ children }: PropsWithChildren) {
  return (
    <header>
      {children}
    </header>
  )
}
