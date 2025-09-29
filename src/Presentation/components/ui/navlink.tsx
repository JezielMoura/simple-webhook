import type { PropsWithChildren, ReactNode } from "react";
import { NavLink } from "react-router";

export type Props = {
  children: ReactNode,
  href: string,
  icon?: string
}

export function NavbarLink({ href, children }: Props) {
  return (
    <NavLink to={href}>
      {children}
    </NavLink>
  )
}