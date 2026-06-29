import {
  Collapse,
  Group,
  Paper,
  Stack,
  Text,
  Title,
  useMantineTheme,
} from "@mantine/core";
import { useDisclosure, useMediaQuery } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import clsx from "clsx";
import { Property } from "csstype";
import { PropsWithChildren, ReactNode } from "react";

interface PageSectionProps {
  title?: string;
  subheader?: ReactNode;
  collapsable?: boolean;
  initialCollapsed?: boolean;
  className?: string;
  maxWidth?: string | number;
  width?: string | number;
  alignChildren?: Property.AlignItems;
  footer?: ReactNode;
}

export default function PageSection({
  title,
  collapsable,
  initialCollapsed = true,
  subheader,
  children,
  className,
  maxWidth,
  width = "100%",
  alignChildren = "center",
  footer,
}: PropsWithChildren<PageSectionProps>) {
  const [opened, { toggle }] = useDisclosure(collapsable && initialCollapsed);
  const { breakpoints } = useMantineTheme();
  const isDesktop = useMediaQuery(`(min-width: ${breakpoints.xs})`);
  return (
    // <Group w={"100%"} justify="center" className="page-section">
    <Paper
      shadow="xl"
      p={24}
      radius={10}
      className={clsx(className, "page-section")}
      maw={maxWidth}
      w={width}
    >
      <Stack w={"100%"} className="page-section__content">
        {(title || collapsable) && (
          <Group
            w={"100%"}
            style={{ alignSelf: "start" }}
            onClick={toggle}
            justify={isDesktop ? undefined : "space-between"}
            className="page-section__title-row"
          >
            <Title size={"h2"}>{title}</Title>
            {collapsable && (opened ? <IconChevronUp /> : <IconChevronDown />)}
          </Group>
        )}
        <Collapse
          expanded={!collapsable || opened}
          w={"100%"}
          className="collapse"
        >
          <Stack align={alignChildren} w={"100%"} gap={"xl"}>
            {typeof subheader === "string" ? (
              <Text mb={"md"}>{subheader}</Text>
            ) : (
              subheader
            )}
            <Stack w={"100%"} align={alignChildren}>
              {children}
            </Stack>
            {footer}
          </Stack>
        </Collapse>
      </Stack>
    </Paper>
    // </Group>
  );
}
