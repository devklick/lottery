import {
  Button,
  Collapse,
  Container,
  Divider,
  Grid,
  Group,
  MultiSelect,
  Select,
  TextInput,
} from "@mantine/core";
import { useForm, schemaResolver } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import {
  LabelledSortByValues,
  SearchGamesRequestFilter,
  searchGamesRequestFilterSchema,
} from "./games.schema";
import {
  allGameStatusesWithLabels,
  allSortDirectionsWithLabel,
} from "../common/schemas";

interface GameFiltersProps {
  initialValues: SearchGamesRequestFilter;
  onUpdateClicked(filters: SearchGamesRequestFilter): void;
}

function GameFilters({ initialValues, onUpdateClicked }: GameFiltersProps) {
  const [opened, { toggle }] = useDisclosure(false);

  const form = useForm<SearchGamesRequestFilter>({
    validate: schemaResolver(searchGamesRequestFilterSchema),
    initialValues,
  });

  const colProps = { span: { xl: 6, lg: 6, md: 6, sm: 6, xs: 12 } };

  return (
    <Container p={0}>
      <Collapse expanded={opened}>
        <form onSubmit={form.onSubmit((data) => onUpdateClicked(data))}>
          <Grid>
            <Grid.Col key={"name"} {...colProps}>
              <TextInput
                label="Game Name"
                {...form.getInputProps("name")}
                style={{ textAlign: "left" }}
              />
            </Grid.Col>
            <Grid.Col key={"status"} {...colProps}>
              <MultiSelect
                label="Game States"
                {...form.getInputProps("gameStatus")}
                style={{ textAlign: "left" }}
                data={Object.values(allGameStatusesWithLabels)}
              />
            </Grid.Col>
            <Grid.Col key="sortby" {...colProps}>
              <Select
                label="Order By"
                {...form.getInputProps("sortBy")}
                style={{ textAlign: "left" }}
                data={Object.values(LabelledSortByValues)}
                allowDeselect={false}
              />
            </Grid.Col>
            <Grid.Col key="sortdir" {...colProps}>
              <Select
                label="Order Direction"
                {...form.getInputProps("sortDirection")}
                style={{ textAlign: "left" }}
                data={Object.values(allSortDirectionsWithLabel)}
                allowDeselect={false}
              />
            </Grid.Col>
          </Grid>
          <Button mb={25} mt={25} type="submit">
            Update Results
          </Button>
        </form>
      </Collapse>
      <Divider
        mb={50}
        labelPosition="right"
        label={
          <Group onClick={toggle}>
            <Button
              size="compact-xs"
              rightSection={
                opened ? (
                  <IconChevronUp size={12} />
                ) : (
                  <IconChevronDown size={12} />
                )
              }
            >
              Filters
            </Button>
          </Group>
        }
      ></Divider>
    </Container>
  );
}

export default GameFilters;
