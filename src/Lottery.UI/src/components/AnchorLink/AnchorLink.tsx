import { Anchor } from "@mantine/core";
import { PropsWithChildren } from "react";
import { Link } from "react-router-dom";

interface AnchorLinkProps {
  to: string;
}

export default function AnchorLink({
  to,
  children,
}: PropsWithChildren<AnchorLinkProps>) {
  return (
    <Anchor component={Link} to={to}>
      {children}
    </Anchor>
  );
}
