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
  return (
    <Container>
      <Stack gap={"xl"}>
        {isPageTitleProps(title) ? (
          <Title style={{ textAlign: title.align ?? "left" }}>
            {title.value}
          </Title>
        ) : (
          title
        )}
        {children}
        {typeof footer === "string" ? <Title>{footer}</Title> : footer}
      </Stack>
    </Container>
  );
}
