import PageSection from "../../components/PageSection";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface YourEntriesProps {}

// eslint-disable-next-line no-empty-pattern
export default function YourEntries({}: YourEntriesProps) {
  return (
    <PageSection
      title="Your Entries"
      subheader="Here you can find all entries you have placed"
      collapsable
    />
  );
}
