import { Container, Stack, Title } from "@mantine/core";
import { Property } from "csstype";
import { PropsWithChildren, ReactNode } from "react";

interface PageTitleProps {
  value: string;
  align?: Property.TextAlign;
}
function isPageTitleProps(
  value: undefined | ReactNode | PageTitleProps,
): value is PageTitleProps {
  return !!value && typeof value === "object";
}

interface PageProps {
  title?: ReactNode | PageTitleProps;
  footer?: ReactNode;
}

export default function Page({
  title,
  footer,
  children,
}: PropsWithChildren<PageProps>) {
  const titleNode = (() => {
    if (isPageTitleProps(title))
      return <Title ta={title.align ?? "left"}>{title.value}</Title>;
    if (typeof title === "string") return <Title ta={"left"}>{title}</Title>;
    return title;
  })();

  return (
    <Container>
      <Stack gap={"xl"}>
        {titleNode}
        {children}
        {typeof footer === "string" ? <Title>{footer}</Title> : footer}
      </Stack>
    </Container>
  );
}
