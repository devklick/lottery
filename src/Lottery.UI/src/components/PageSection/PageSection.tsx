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
import { PropsWithChildren, ReactNode } from "react";

interface PageSectionProps {
  title: string;
  subheader?: ReactNode;
  collapsable?: boolean;
  initialCollapsed?: boolean;
  className?: string;
}

export default function PageSection({
  title,
  collapsable,
  initialCollapsed = true,
  subheader,
  children,
  className,
}: PropsWithChildren<PageSectionProps>) {
  const [opened, { toggle }] = useDisclosure(collapsable && initialCollapsed);
  const { breakpoints } = useMantineTheme();
  const isDesktop = useMediaQuery(`(min-width: ${breakpoints.xs})`);
  return (
    <Paper
      shadow="xl"
      p={24}
      radius={10}
      className={clsx(className, "page-section")}
    >
      <Stack w={"100%"}>
        <Group
          w={"100%"}
          style={{ alignSelf: "start" }}
          onClick={toggle}
          mb={"md"}
          justify={isDesktop ? undefined : "space-between"}
        >
          <Title size={"h2"}>{title}</Title>
          {collapsable && (opened ? <IconChevronUp /> : <IconChevronDown />)}
        </Group>
        <Collapse expanded={opened}>
          <>
            {typeof subheader === "string" ? (
              <Text mb={"md"}>{subheader}</Text>
            ) : (
              subheader
            )}
            {children}
          </>
        </Collapse>
      </Stack>
    </Paper>
  );
}
