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
import { PropsWithChildren } from "react";

interface PageSectionProps {
  title: string;
  subheader?: string;
  collapsable?: boolean;
  initialCollapsed?: boolean;
}

export default function PageSection({
  title,
  collapsable = true,
  initialCollapsed = true,
  subheader,
  children,
}: PropsWithChildren<PageSectionProps>) {
  const [opened, { toggle }] = useDisclosure(collapsable && initialCollapsed);
  const { breakpoints } = useMantineTheme();
  const isDesktop = useMediaQuery(`(min-width: ${breakpoints.xs})`);
  return (
    <Paper shadow="xl" p={24} radius={10}>
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
            <Text mb={"md"}>{subheader}</Text>
            {children}
          </>
        </Collapse>
      </Stack>
    </Paper>
  );
}
